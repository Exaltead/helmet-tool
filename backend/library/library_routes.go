package library

import (
	"fmt"
	"haasteikko/backend/auth"
	"haasteikko/backend/shared"
	"time"

	"github.com/google/uuid"
)

func ConfigureLibraryRoutes() {
	shared.ConfigureUserContentRoutes("library", shared.UserContentRouteConfig[LibraryItem]{
		Repository:          &LibraryRepository{},
		AuthValidator:       auth.JWTValidator{},
		CreateNewFromParsed: createNewFromParsedItem,
		ValidateItem:        validateItem,
		MergeFromExisting:   mergeFromExisting,
	})
}

func validateItem(item LibraryItem) error {
	if item.ActivatedChallengeIds == nil {
		return fmt.Errorf("activatedChallengeIds must not be nil")
	}
	if item.Kind != "Book" {
		return fmt.Errorf("kind must be Book")
	}

	if item.Kind == "Book" && item.Book == nil {
		return fmt.Errorf("book must not be nil")
	}
	return nil
}

func mergeFromExisting(item LibraryItem, existingItem LibraryItem) (LibraryItem, error) {
	item.UserId = existingItem.UserId
	item.AddDate = existingItem.AddDate

	return item, nil
}

func createNewFromParsedItem(item LibraryItem, userId string) (LibraryItem, error) {
	item.UserId = userId
	item.Id = uuid.New().String()
	item.AddDate = time.Now()

	item.ActivatedChallengeIds = []string{} // TODO: get these from currently active challenges

	return item, nil
}
