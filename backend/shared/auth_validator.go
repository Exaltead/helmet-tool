package shared

import "net/http"

type AuthValidator interface {
	GetAuthenticatedUserId(r *http.Request) (string, error)
}
