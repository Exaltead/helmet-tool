package sharedchallenges

import (
	"fmt"
	"haasteikko/backend/auth"
	"haasteikko/backend/shared"

	"github.com/google/uuid"
)

func ConfigureShareChallengeRoutes() {
	shared.ConfigureSharedContentRoutes("challenge", shared.SharedContentRouteConfig[SharedChallenge]{
		Repository:          &SharedChallengeRepository{},
		AuthValidator:       auth.JWTValidator{},
		CreateNewFromParsed: createNewFromParsedItem,
		ValidateItem:        validateItem,
	})
}

func validateItem(item SharedChallenge) error {
	if item.Questions == nil {
		return fmt.Errorf("questions must not be nil")
	}
	// TODO: Add more validation logic as needed
	return nil
}

func createNewFromParsedItem(item SharedChallenge, userId string) (SharedChallenge, error) {

	if item.Questions == nil {
		item.Questions = []Question{}
	}

	item.Id = uuid.NewString()
	return item, nil
}
