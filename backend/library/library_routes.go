package library

import (
	"fmt"
	"haasteikko/backend/auth"
	"haasteikko/backend/shared"
	sharedChallenges "haasteikko/backend/sharedChallenges"
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

	switch item.Kind {
	case "Book":
		if item.Book == nil {
			return fmt.Errorf("book must not be nil")
		}
	case "Game":
		if item.Game == nil {
			return fmt.Errorf("game must not be nil")
		}
	default:
		return fmt.Errorf("item must have valid kind")
	}

	return nil
}

func mergeFromExisting(item LibraryItem, existingItem LibraryItem) (LibraryItem, error) {
	item.UserId = existingItem.UserId
	item.AddDate = existingItem.AddDate

	return item, nil
}

func getCurrentActiveChallengesForKind(kind string) ([]string, error) {
	challengesRepo := sharedChallenges.SharedChallengeRepository{}

	current, err := challengesRepo.GetAll()
	if err != nil {
		return []string{}, err
	}

	result := []string{}
	for _, challenge := range current {
		if challenge.Status == "active" && challenge.TargetMedia == kind {
			result = append(result, challenge.Id)
		}
	}

	return result, nil
}

func createNewFromParsedItem(item LibraryItem, userId string) (LibraryItem, error) {

	item.UserId = userId
	item.Id = uuid.New().String()
	item.AddDate = time.Now()

	challengeIds, err := getCurrentActiveChallengesForKind(item.Kind)

	if err != nil {
		return LibraryItem{}, err
	}

	item.ActivatedChallengeIds = challengeIds
	return item, nil
}
