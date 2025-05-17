package shared

import (
	"bytes"
	"encoding/json"
	testingtools "haasteikko/backend/testing_tools"
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/stretchr/testify/assert"
)

type TestItem struct {
	Id           string `json:"id"`
	UserId       string `json:"userId"`
	OtherContent string `json:"otherContent"`
}

func (t TestItem) GetId() string {
	return t.Id
}

func (t TestItem) GetUserId() string {
	return t.UserId
}

func createNewFromParsedItem(item TestItem, userId string) (TestItem, error) {
	item.UserId = userId
	return item, nil
}

func validateItem(item TestItem) error {
	return nil
}

func TestAllRoutesAuthentication(t *testing.T) {
	routeConfig := UserContentRouteConfig[TestItem]{
		Repository: &testingtools.MockRepository[TestItem]{},
		AuthValidator: testingtools.MockValidator{
			UserId: ""},
		CreateNewFromParsed: createNewFromParsedItem,
		ValidateItem:        validateItem,
	}

	t.Run("GET /api/test", func(t *testing.T) {
		req := httptest.NewRequest(http.MethodGet, "/api/test", nil)
		w := httptest.NewRecorder()
		getItemsFromRepository(w, req, routeConfig)

		assert.Equal(t, http.StatusUnauthorized, w.Code)
	})

	t.Run("GET /api/test/12345", func(t *testing.T) {
		req := httptest.NewRequest(http.MethodGet, "/api/test/12345", nil)
		w := httptest.NewRecorder()
		getItemByIdFromRepository(w, req, routeConfig, "12345")

		assert.Equal(t, http.StatusUnauthorized, w.Code)
	})

	t.Run("POST /api/test", func(t *testing.T) {
		req := httptest.NewRequest(http.MethodPost, "/api/test", nil)
		w := httptest.NewRecorder()
		postNewItemToLibrary(w, req, routeConfig)

		assert.Equal(t, http.StatusUnauthorized, w.Code)
	})
	t.Run("DELETE /api/test/12345", func(t *testing.T) {
		req := httptest.NewRequest(http.MethodDelete, "/api/test/12345", nil)
		w := httptest.NewRecorder()
		deleteItemFromLibrary(w, req, routeConfig, "12345")

		assert.Equal(t, http.StatusUnauthorized, w.Code)
	})

	t.Run("PUT /api/test/12345", func(t *testing.T) {
		req := httptest.NewRequest(http.MethodPut, "/api/test/12345", nil)
		w := httptest.NewRecorder()
		updateItemInLibrary(w, req, routeConfig)

		assert.Equal(t, http.StatusUnauthorized, w.Code)
	})
}

func TestGetItemsRoute(t *testing.T) {
	testItems := []TestItem{
		{Id: "1", UserId: "123455", OtherContent: "test1"},
		{Id: "2", UserId: "123455", OtherContent: "test2"},
	}
	routeConfig := UserContentRouteConfig[TestItem]{
		Repository: &testingtools.MockRepository[TestItem]{
			GetAllItems: testItems},
		AuthValidator: testingtools.MockValidator{
			UserId: "123455"},
		CreateNewFromParsed: createNewFromParsedItem,
		ValidateItem:        validateItem,
	}

	t.Run("Positive case - get all items", func(t *testing.T) {
		req := httptest.NewRequest(http.MethodGet, "/api/test", nil)
		w := httptest.NewRecorder()
		getItemsFromRepository(w, req, routeConfig)

		assert.Equal(t, http.StatusOK, w.Code)

		content, err := testingtools.GetBodyJsonContent[[]TestItem](w)
		assert.NoError(t, err)
		assert.Equal(t, testItems, content)

	})
}

func TestGetItemByIdRoute(t *testing.T) {
	testItem := TestItem{Id: "1", UserId: "123455", OtherContent: "test1"}
	routeConfig := UserContentRouteConfig[TestItem]{
		Repository: &testingtools.MockRepository[TestItem]{
			GetItem: testItem,
		},
		AuthValidator: testingtools.MockValidator{
			UserId: "123455"},
		CreateNewFromParsed: createNewFromParsedItem,
		ValidateItem:        validateItem,
	}

	t.Run("Positive case - get item by id", func(t *testing.T) {
		req := httptest.NewRequest(http.MethodGet, "/api/test/1", nil)
		w := httptest.NewRecorder()
		getItemByIdFromRepository(w, req, routeConfig, "1")

		assert.Equal(t, http.StatusOK, w.Code)
		content, err := testingtools.GetBodyJsonContent[TestItem](w)
		assert.NoError(t, err)
		assert.Equal(t, testItem, content)
	})
}

func TestPostNewItemToLibraryRoute(t *testing.T) {
	testItem := TestItem{Id: "1", UserId: "123455", OtherContent: "test1"}
	routeConfig := UserContentRouteConfig[TestItem]{
		Repository: &testingtools.MockRepository[TestItem]{
			CreateItemId: "1",
		},
		AuthValidator: testingtools.MockValidator{
			UserId: "123455"},
		CreateNewFromParsed: createNewFromParsedItem,
		ValidateItem:        validateItem,
	}

	t.Run("Positive case - post new item", func(t *testing.T) {
		body, _ := json.Marshal(testItem)
		req := httptest.NewRequest(http.MethodPost, "/api/test", bytes.NewReader(body))
		w := httptest.NewRecorder()
		postNewItemToLibrary(w, req, routeConfig)

		assert.Equal(t, http.StatusCreated, w.Code)
		content, err := testingtools.GetBodyJsonContent[map[string]any](w)
		assert.NoError(t, err)
		assert.Equal(t, "1", content["id"])
	})
}

func TestDeleteItemFromLibraryRoute(t *testing.T) {
	routeConfig := UserContentRouteConfig[TestItem]{
		Repository: &testingtools.MockRepository[TestItem]{},
		AuthValidator: testingtools.MockValidator{
			UserId: "123455"},
		CreateNewFromParsed: createNewFromParsedItem,
		ValidateItem:        validateItem,
	}

	t.Run("Positive case - delete item", func(t *testing.T) {
		req := httptest.NewRequest(http.MethodDelete, "/api/test/1", nil)
		w := httptest.NewRecorder()
		deleteItemFromLibrary(w, req, routeConfig, "1")

		assert.Equal(t, http.StatusNoContent, w.Code)
	})
}

func TestUpdateItemInLibraryRoute(t *testing.T) {
	testItem := TestItem{Id: "1", UserId: "123455", OtherContent: "test1"}
	routeConfig := UserContentRouteConfig[TestItem]{
		Repository: &testingtools.MockRepository[TestItem]{},
		AuthValidator: testingtools.MockValidator{
			UserId: "123455"},
		CreateNewFromParsed: createNewFromParsedItem,
		ValidateItem:        validateItem,
	}

	t.Run("Positive case - update item", func(t *testing.T) {
		body, _ := json.Marshal(testItem)
		req := httptest.NewRequest(http.MethodPut, "/api/test/1", bytes.NewReader(body))
		w := httptest.NewRecorder()
		updateItemInLibrary(w, req, routeConfig)

		assert.Equal(t, http.StatusOK, w.Code)
	})
}
