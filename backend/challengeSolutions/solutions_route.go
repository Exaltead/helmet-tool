package challengeSolutions

import (
	"fmt"
	"haasteikko/backend/auth"
	"haasteikko/backend/shared"
	"time"

	"github.com/google/uuid"
)

func ConfigureSolutionRoutes() {
	shared.ConfigureUserContentRoutes(
		"solution",
		shared.UserContentRouteConfig[SolutionSet]{
			Repository:          &SolutionRepository{},
			AuthValidator:       auth.JWTValidator{},
			CreateNewFromParsed: createNewFromParsedItem,
			ValidateItem:        validateItem,
			MergeFromExisting:   mergeFromExisting,
		})
}

func mergeFromExisting(item SolutionSet, existingItem SolutionSet) (SolutionSet, error) {
	item.UserId = existingItem.UserId
	item.AddDate = existingItem.AddDate


	return item, nil
}

func createNewFromParsedItem(item SolutionSet, userId string) (SolutionSet, error) {
	item.UserId = userId
	item.Id = uuid.New().String()
	item.AddDate = time.Now()
	if item.Solutions == nil {
		item.Solutions = []QuestionSolution{}
	}

	return item, nil
}

func validateItem(item SolutionSet) error {
	if item.UserId == "" {
		return fmt.Errorf("userId must not be empty")
	}
	if item.ChallengeId == "" {
		return fmt.Errorf("challengeId must not be empty")
	}
	if item.Solutions == nil {
		return fmt.Errorf("answers must not be nil")
	}

	return nil
}
