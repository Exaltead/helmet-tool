package auth

import (
	"encoding/base64"
	"errors"
	"log"
	"net/http"
	"os"
	"strings"
	"time"

	"github.com/golang-jwt/jwt/v5"
)

type JWTValidator struct{}

func (b JWTValidator) GetAuthenticatedUserId(r *http.Request) (string, error) {
	tokenString := r.Header.Get("Authorization")
	tokenString = strings.TrimPrefix(tokenString, "Bearer ")
	if tokenString == "" {
		return "", errors.New("missing token")
	}

	claims := &jwt.RegisteredClaims{}
	token, err := jwt.ParseWithClaims(tokenString, claims, func(token *jwt.Token) (any, error) {
		return getSecretBytes(), nil
	})
	if err != nil {
		return "", err
	}
	if !token.Valid {
		return "", errors.New("invalid token")
	}

	if exp, err := token.Claims.GetExpirationTime(); err == nil && exp.Time.Before(time.Now()) {
		return "", errors.New("token expired")
	}

	return claims.Subject, nil
}

func CreateToken(username string, userId string) (string, error) {
	secretBytes := getSecretBytes()

	token := jwt.NewWithClaims(jwt.SigningMethodHS256, jwt.MapClaims{
		"username": username,
		"sub":      userId,
		"exp":      jwt.NewNumericDate(time.Now().Add(24 * time.Hour)),
	})

	tokenString, err := token.SignedString(secretBytes)

	if err == nil {
		return tokenString, nil
	}

	log.Fatalln(err)
	return tokenString, errors.New("failed to create token")
}

func getSecretBytes() []byte {
	secretBytes, err := base64.StdEncoding.DecodeString(os.Getenv("SECRET_KEY"))
	if err != nil {
		log.Fatalf("Failed to decode base64: %v", err)
	}
	return secretBytes
}
