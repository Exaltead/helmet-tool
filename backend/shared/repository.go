package shared

import "net/url"

type UserContent interface {
	GetId() string
	GetUserId() string
}

type UserContentRepository[T UserContent] interface {
	GetAll(userId string, query url.Values) ([]T, error)
	GetById(id string, userId string) (T, error)
	DeleteById(id string, userId string) error
	Create(item T, userId string) (string, error)
	Update(item T, userId string) error
}

type RepositoryContent interface {
	GetId() string
}

type SharedContentRepository[T any] interface {
	GetAll() ([]T, error)
	GetById(id string) (T, error)
	DeleteById(id string) error
	Create(item T) (string, error)
	Update(item T) error
}
