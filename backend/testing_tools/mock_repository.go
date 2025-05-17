package testingtools

import "net/url"

// MockRepository is a mock implementation of a repository for testing purposes.
type MockRepository[T any] struct {
	GetAllItems     []T
	GetAllError     *error
	GetItem         T
	GetItemError    *error
	DeleteItemError *error
	CreateItemId    string
	CreateItemError *error
	CreateItemCall  T
	UpdateItemError *error
}

// GetAll retrieves all items for a given user ID.
func (m *MockRepository[T]) GetAll(userId string, query url.Values) ([]T, error) {
	if m.GetAllError != nil {
		return nil, *m.GetAllError
	}
	return m.GetAllItems, nil
}

// GetById retrieves a single item by its ID and user ID.
func (m *MockRepository[T]) GetById(id string, userId string) (T, error) {
	if m.GetItemError != nil {
		return *new(T), *m.GetItemError
	}
	return m.GetItem, nil
}

// DeleteById deletes an item by its ID and user ID.
func (m *MockRepository[T]) DeleteById(id string, userId string) error {
	if m.DeleteItemError != nil {
		return *m.DeleteItemError
	}
	return nil
}

// Create adds a new item and returns its ID.
func (m *MockRepository[T]) Create(item T, userId string) (string, error) {
	if m.CreateItemError != nil {
		return "", *m.CreateItemError
	}
	m.CreateItemCall = item
	return m.CreateItemId, nil
}

// Update modifies an existing item.
func (m *MockRepository[T]) Update(item T, userId string) error {
	if m.UpdateItemError != nil {
		return *m.UpdateItemError
	}
	return nil
}
