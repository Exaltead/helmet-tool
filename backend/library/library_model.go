package library

import "time"

type LibraryBook struct {
	Title      string  `json:"title"`
	Author     string  `json:"author"`
	Translator *string `json:"translator,omitempty"`
}

type LibraryItem struct {
	Kind                  string       `json:"kind"`
	Id                    string       `json:"id"`
	UserId                string       `json:"userId"`
	AddDate               time.Time    `json:"addDate"`
	ActivatedChallengeIds []string     `json:"activatedChallengeIds"`
	Book                  *LibraryBook `json:"book,omitempty"`
}

func (l LibraryItem) GetId() string {
	return l.Id
}

func (l LibraryItem) GetUserId() string {
	return l.UserId
}