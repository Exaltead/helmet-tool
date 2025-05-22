package auth

import (
	"crypto/sha256"
	"encoding/base64"
	"encoding/json"
	"log"
	"net/http"

	"haasteikko/backend/shared"
)

type LoginRequest struct {
	Username string `json:"username"`
	Password string `json:"password"`
}

func LogIn(w http.ResponseWriter, r *http.Request) {
	var loginRequest LoginRequest
	if err := shared.ValidateJsonRequest(w, r, &loginRequest); err != nil {
		return
	}

	user, err := getUserInfo(loginRequest.Username)
	if err != nil {
		http.Error(w, "Internal error 1 "+err.Error(), http.StatusInternalServerError)
		return
	}
	if user == nil {
		http.Error(w, "User not found", http.StatusUnauthorized)
		return
	}

	if user.PasswordHash != getSha256Hash(loginRequest.Password) {
		log.Default().Println("User password is invalid")
		http.Error(w, "User not found 2", http.StatusUnauthorized)
		return
	}

	if token, err := CreateToken(loginRequest.Username, user.Id); err == nil {
		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(map[string]any{
			"token": token,
		})
	} else {
		http.Error(w, "Internal error 2", http.StatusInternalServerError)
	}
}

func getSha256Hash(content string) string {
	hash := sha256.Sum256([]byte(content))
	return base64.StdEncoding.EncodeToString(hash[:])

}
