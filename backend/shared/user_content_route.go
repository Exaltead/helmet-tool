package shared

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"
)

type UserContentRouteConfig[T UserContent] struct {
	Repository          UserContentRepository[T]
	AuthValidator       AuthValidator
	CreateNewFromParsed func(item T, userId string) (T, error)
	ValidateItem        func(item T) error
	MergeFromExisting   func(item T, existingItem T) (T, error)
}

func ConfigureUserContentRoutes[T UserContent](entityName string, routeConfig UserContentRouteConfig[T]) {
	http.HandleFunc(fmt.Sprintf("GET /api/%s", entityName), getItemsRoute(routeConfig))
	http.HandleFunc(fmt.Sprintf("GET /api/%s/{id}", entityName), getItemByIdRoute(routeConfig))
	http.HandleFunc(fmt.Sprintf("POST /api/%s", entityName), postItemsRoute(routeConfig))
	http.HandleFunc(fmt.Sprintf("DELETE /api/%s/{id}", entityName), deleteItemsRoute(routeConfig))
	http.HandleFunc(fmt.Sprintf("PUT /api/%s/{id}", entityName), updateItemRoute(routeConfig))

}

func getItemsRoute[T UserContent](c UserContentRouteConfig[T]) func(w http.ResponseWriter, r *http.Request) {
	getItems := func(w http.ResponseWriter, r *http.Request) {
		getItemsFromRepository(w, r, c)
	}
	return getItems
}

func postItemsRoute[T UserContent](c UserContentRouteConfig[T]) func(w http.ResponseWriter, r *http.Request) {
	postItems := func(w http.ResponseWriter, r *http.Request) {
		postNewItemToLibrary(w, r, c)
	}
	return postItems
}

func deleteItemsRoute[T UserContent](c UserContentRouteConfig[T]) func(w http.ResponseWriter, r *http.Request) {
	deleteItems := func(w http.ResponseWriter, r *http.Request) {
		id := r.PathValue("id")
		if id == "" {
			http.Error(w, "Id is required", http.StatusBadRequest)
			return
		}
		deleteItemFromLibrary(w, r, c, id)
	}
	return deleteItems
}

func getItemByIdRoute[T UserContent](c UserContentRouteConfig[T]) func(w http.ResponseWriter, r *http.Request) {
	getItemById := func(w http.ResponseWriter, r *http.Request) {
		id := r.PathValue("id")
		if id == "" {
			http.Error(w, "Id is required", http.StatusBadRequest)
			return
		}
		getItemByIdFromRepository(w, r, c, id)
	}
	return getItemById
}

func updateItemRoute[T UserContent](c UserContentRouteConfig[T]) func(w http.ResponseWriter, r *http.Request) {
	updateItem := func(w http.ResponseWriter, r *http.Request) {
		updateItemInLibrary(w, r, c)
	}
	return updateItem
}

func getItemsFromRepository[T UserContent](w http.ResponseWriter, r *http.Request,
	c UserContentRouteConfig[T],
) {
	userId, err := c.AuthValidator.GetAuthenticatedUserId(r)
	if err != nil {
		http.Error(w, "Unauthorized"+err.Error(), http.StatusUnauthorized)
		return
	}

	query := r.URL.Query()
	if items, err := c.Repository.GetAll(userId, query); err != nil {
		log.Default().Println("Failed to get items", err)
		http.Error(w, "Failed to get items", 500)
	} else {
		json.NewEncoder(w).Encode(items)
	}

}

func postNewItemToLibrary[T UserContent](w http.ResponseWriter, r *http.Request,
	routeConfig UserContentRouteConfig[T]) {

	userId, err := routeConfig.AuthValidator.GetAuthenticatedUserId(r)
	if err != nil {
		http.Error(w, "Unauthorized"+err.Error(), http.StatusUnauthorized)
		return
	}

	var parsed T
	if err := json.NewDecoder(r.Body).Decode(&parsed); err != nil {
		http.Error(w, "Failed to decode item"+err.Error(), 400)
		return
	}

	item, err := routeConfig.CreateNewFromParsed(parsed, userId)
	if err != nil {
		log.Default().Println("Failed to create item", err)
		http.Error(w, "Failed to create item", 500)
		return
	}
	if err := routeConfig.ValidateItem(item); err != nil {
		log.Default().Println("Failed to validate item", err)
		http.Error(w, "Failed to validate item", 400)
		return
	}

	if id, err := routeConfig.Repository.Create(item, userId); err != nil {
		log.Default().Println("Failed to add item", err)
		http.Error(w, "Failed to add item", 500)
	} else {
		w.WriteHeader(http.StatusCreated)
		json.NewEncoder(w).Encode(map[string]any{
			"id": id})
	}
}

func deleteItemFromLibrary[T UserContent](w http.ResponseWriter, r *http.Request,
	routeConfig UserContentRouteConfig[T], id string) {

	userId, err := routeConfig.AuthValidator.GetAuthenticatedUserId(r)
	if err != nil {
		http.Error(w, "Unauthorized"+err.Error(), http.StatusUnauthorized)
		return
	}

	if err := routeConfig.Repository.DeleteById(id, userId); err != nil {
		http.Error(w, "Failed to delete item"+err.Error(), 500)
	} else {
		w.WriteHeader(http.StatusOK)
	}
}

func getItemByIdFromRepository[T UserContent](w http.ResponseWriter, r *http.Request,
	routeConfig UserContentRouteConfig[T], id string) {

	userId, err := routeConfig.AuthValidator.GetAuthenticatedUserId(r)
	if err != nil {
		http.Error(w, "Unauthorized"+err.Error(), http.StatusUnauthorized)
		return
	}

	if item, err := routeConfig.Repository.GetById(id, userId); err != nil {
		if err.Error() == "item not found" {
			http.Error(w, "Item not found", 404)
			return
		}
		http.Error(w, "Failed to get item"+err.Error(), 500)
	} else {
		json.NewEncoder(w).Encode(item)
	}
}

func updateItemInLibrary[T UserContent](w http.ResponseWriter, r *http.Request,
	routeConfig UserContentRouteConfig[T]) {

	userId, err := routeConfig.AuthValidator.GetAuthenticatedUserId(r)
	if err != nil {
		http.Error(w, "Unauthorized"+err.Error(), http.StatusUnauthorized)
		return
	}

	var item T
	if err := json.NewDecoder(r.Body).Decode(&item); err != nil {
		http.Error(w, "Failed to decode item"+err.Error(), 400)
		return
	}

	existingItem, err := routeConfig.Repository.GetById(item.GetId(), userId)
	if err != nil {
		log.Default().Println("Failed to get existing item", err)
		http.Error(w, "Failed to get existing item", 404)
		return
	}

	if existingItem.GetUserId() != userId {
		log.Default().Println("UserId does not match")
		http.Error(w, "UserId does not match", http.StatusForbidden)
		return
	}

	item, err = routeConfig.MergeFromExisting(item, existingItem)
	if err != nil {
		log.Default().Println("Failed to merge item", err)
		http.Error(w, "Failed to merge item", 500)
		return
	}

	if err := routeConfig.ValidateItem(item); err != nil {
		log.Default().Println("Failed to validate item", err)
		http.Error(w, "Failed to validate item", 400)
		return
	}

	if err := routeConfig.Repository.Update(item, userId); err != nil {
		http.Error(w, "Failed to update item"+err.Error(), 500)
	} else {
		w.WriteHeader(http.StatusOK)
	}
}
