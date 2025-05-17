package challengeAnswers

import "time"

type Answer struct {
	Kind       string `json:"kind"`
	Id         string `json:"id"`
	QuestionId string `json:"questionId"`
	Answered   bool   `json:"answered"`
	Answer     string `json:"answer"`
	ItemId     string `json:"itemId"`
}

type ChallengeAnswerSet struct {
	Id          string    `json:"id"`
	UserId      string    `json:"userId"`
	ChallengeId string    `json:"challengeId"`
	AddDate     time.Time `json:"addDate"`
	Answers     []Answer  `json:"answers"`
	ItemId     string `json:"itemId"`
}

func (l ChallengeAnswerSet) GetId() string {
	return l.Id
}

func (l ChallengeAnswerSet) GetUserId() string {
	return l.UserId
}
