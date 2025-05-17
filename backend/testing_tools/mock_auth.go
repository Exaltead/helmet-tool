package testingtools

import (
	"errors"
	"net/http"
)

type MockValidator struct {
	UserId string
}

func (m MockValidator) GetAuthenticatedUserId(r *http.Request) (string, error) {
	if m.UserId != "" {
		return m.UserId, nil
	}

	return "", errors.New("unauthorized")
}
