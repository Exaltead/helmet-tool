package shared

import (
	"encoding/json"
	"errors"
	"net/http"
)

func ValidateJsonRequest(w http.ResponseWriter, r *http.Request, target any) error {
	if !(r.Method == http.MethodPost || r.Method == http.MethodPut) {
		http.Error(w, "Method not allowed", http.StatusMethodNotAllowed)
		return errors.ErrUnsupported
	}
	if r.Header.Get("Content-Type") != "application/json" {
		http.Error(w, "Content-Type must be application/json", http.StatusUnsupportedMediaType)
		return errors.ErrUnsupported
	}

	r.Body = http.MaxBytesReader(w, r.Body, 1<<20) // 1 MB limit
	if err := json.NewDecoder(r.Body).Decode(&target); err != nil {
		http.Error(w, "Invalid JSON: "+err.Error(), http.StatusBadRequest)
		return errors.ErrUnsupported
	}

	return nil
}
