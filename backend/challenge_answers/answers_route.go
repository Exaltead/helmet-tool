package challengeAnswers

import (
	"fmt"
	"haasteikko/backend/auth"
	"haasteikko/backend/shared"
	"strings"
	"time"

	"github.com/google/uuid"
)

func ConfigureAnswersRoutes() {
	shared.ConfigureUserContentRoutes(
		"answer",
		shared.UserContentRouteConfig[ChallengeAnswerSet]{
			Repository:          &ChallengeAnswerRepository{},
			AuthValidator:       auth.JWTValidator{},
			CreateNewFromParsed: createNewFromParsedItem,
			ValidateItem:        validateItem,
			MergeFromExisting:   mergeFromExisting,
		})
}

func mergeFromExisting(item ChallengeAnswerSet, existingItem ChallengeAnswerSet) (ChallengeAnswerSet, error) {
	item.UserId = existingItem.UserId
	item.AddDate = existingItem.AddDate

	answers, err := updateAnswerState(item.Answers)
	if err != nil {
		return ChallengeAnswerSet{}, err
	}
	item.Answers = answers
	return item, nil
}

func createNewFromParsedItem(item ChallengeAnswerSet, userId string) (ChallengeAnswerSet, error) {
	item.UserId = userId
	item.Id = uuid.New().String()
	item.AddDate = time.Now()
	if item.Answers == nil {
		item.Answers = []Answer{}
	}

	answers, err := updateAnswerState(item.Answers)
	if err != nil {
		return ChallengeAnswerSet{}, err
	}
	item.Answers = answers

	return item, nil
}

func validateItem(item ChallengeAnswerSet) error {
	if item.UserId == "" {
		return fmt.Errorf("userId must not be empty")
	}
	if item.ChallengeId == "" {
		return fmt.Errorf("challengeId must not be empty")
	}
	if item.Answers == nil {
		return fmt.Errorf("answers must not be nil")
	}
	if len(item.Answers) == 0 {
		return fmt.Errorf("answers must not be empty")
	}

	return nil
}

func updateAnswerState(answers []Answer) ([]Answer, error) {
	result := make([]Answer, len(answers))
	for i, element := range answers {
		element.Answer = strings.TrimSpace(element.Answer)
		if element.Id == "" {
			element.Id = uuid.NewString()
		}
		if element.Kind == "Boolean" {
			element.Answered = true
		} else if element.Kind == "TextInput" {
			element.Answered = element.Answer != ""
		} else {
			return nil, fmt.Errorf("unknown answer kind: %s", element.Kind)
		}
		result[i] = element

	}
	return result, nil

}
