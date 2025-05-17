package testingtools

import (
	"encoding/json"
	"io"
	"net/http/httptest"
	"testing"
)

func GetBodyJsonContent[T any](w *httptest.ResponseRecorder) (T, error) {
	res := w.Result()

	defer res.Body.Close()

	if data, err := io.ReadAll(res.Body); err == nil {
		item := new(T)
		if err := json.Unmarshal(data, &item); err == nil {
			return *item, nil
		} else {
			return *item, err
		}

	} else {
		return *new(T), nil
	}

}

func AssertStatusCode(t *testing.T, w *httptest.ResponseRecorder, expected int) {
	t.Helper()
	if w.Code != expected {
		t.Errorf("Expected status code %d, got %d", expected, w.Code)
	}
}
