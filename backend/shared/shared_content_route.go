// Add add, update, list
package shared

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"
)

type SharedContentRouteConfig[T RepositoryContent] struct {
	Repository          SharedContentRepository[T]
	AuthValidator       AuthValidator
	CreateNewFromParsed func(item T, userId string) (T, error)
	ValidateItem        func(item T) error
}

func ConfigureSharedContentRoutes[T RepositoryContent](entityName string, routeConfig SharedContentRouteConfig[T]) {
	http.HandleFunc(fmt.Sprintf("GET /api/%s", entityName), getSharedItemsRoute(routeConfig))
	http.HandleFunc(fmt.Sprintf("POST /api/%s", entityName), postSharedItemRoute(routeConfig))
	http.HandleFunc(fmt.Sprintf("PUT /api/%s/{id}", entityName), updateSharedItemRoute(routeConfig))
}

func getSharedItemsRoute[T RepositoryContent](c SharedContentRouteConfig[T]) func(w http.ResponseWriter, r *http.Request) {
	getItems := func(w http.ResponseWriter, r *http.Request) {
		getSharedItemsFromRepository(w, r, c)
	}
	return getItems
}

func postSharedItemRoute[T RepositoryContent](c SharedContentRouteConfig[T]) func(w http.ResponseWriter, r *http.Request) {
	postItems := func(w http.ResponseWriter, r *http.Request) {
		postNewSharedItem(w, r, c)
	}
	return postItems
}

func updateSharedItemRoute[T RepositoryContent](c SharedContentRouteConfig[T]) func(w http.ResponseWriter, r *http.Request) {
	updateItem := func(w http.ResponseWriter, r *http.Request) {
		updateSharedItem(w, r, c)
	}
	return updateItem
}

func getSharedItemsFromRepository[T RepositoryContent](w http.ResponseWriter, r *http.Request,
	c SharedContentRouteConfig[T],
) {
	_, err := c.AuthValidator.GetAuthenticatedUserId(r)
	if err != nil {
		http.Error(w, "Unauthorized"+err.Error(), http.StatusUnauthorized)
		return
	}
	if items, err := c.Repository.GetAll(); err != nil {
		log.Default().Println("Failed to get items", err)
		http.Error(w, "Failed to get items", 500)
	} else {
		json.NewEncoder(w).Encode(items)
	}

}

func postNewSharedItem[T RepositoryContent](w http.ResponseWriter, r *http.Request,
	routeConfig SharedContentRouteConfig[T]) {

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

	if id, err := routeConfig.Repository.Create(item); err != nil {
		log.Default().Println("Failed to add item", err)
		http.Error(w, "Failed to add item", 500)
	} else {
		w.WriteHeader(http.StatusCreated)
		json.NewEncoder(w).Encode(map[string]any{
			"id": id})
	}
}

func updateSharedItem[T RepositoryContent](w http.ResponseWriter, r *http.Request,
	routeConfig SharedContentRouteConfig[T]) {

	_, err := routeConfig.AuthValidator.GetAuthenticatedUserId(r)
	if err != nil {
		http.Error(w, "Unauthorized"+err.Error(), http.StatusUnauthorized)
		return
	}

	var item T
	if err := json.NewDecoder(r.Body).Decode(&item); err != nil {
		http.Error(w, "Failed to decode item"+err.Error(), 400)
		return
	}

	if err := routeConfig.ValidateItem(item); err != nil {
		log.Default().Println("Failed to validate item", err)
		http.Error(w, "Failed to validate item", 400)
		return
	}

	if err := routeConfig.Repository.Update(item); err != nil {
		http.Error(w, "Failed to update item"+err.Error(), 500)
	} else {
		w.WriteHeader(http.StatusOK)
	}
}
