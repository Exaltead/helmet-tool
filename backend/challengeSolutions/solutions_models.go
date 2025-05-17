package challengeSolutions

import "time"

type QuestionSolution struct {
	QuestionId            string `json:"questionId"`
	SingleAnswerItemId    string   `json:"singleAnswerItemId"`
	MultipleAnswerItemIds []string `json:"multipleAnswerItemIds"`
	ItemId                string `json:"itemId"`
}

type SolutionSet struct {
	Id          string             `json:"id"`
	UserId      string             `json:"userId"`
	ChallengeId string             `json:"challengeId"`
	AddDate     time.Time          `json:"addDate"`
	Solutions   []QuestionSolution `json:"solutions"`
}

func (l SolutionSet) GetId() string {
	return l.Id
}

func (l SolutionSet) GetUserId() string {
	return l.UserId
}
