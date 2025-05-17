package library

import (
	"haasteikko/backend/shared"
	"net/url"

	"github.com/Azure/azure-sdk-for-go/sdk/data/azcosmos"
)

const containerName string = "library"

type LibraryRepository struct {
}

func (r *LibraryRepository) GetAll(userId string, queryParams url.Values) ([]LibraryItem, error) {
	query := "SELECT * FROM c WHERE c.userId = @userId"
	queryOptions := azcosmos.QueryOptions{
		QueryParameters: []azcosmos.QueryParameter{
			{Name: "@userId", Value: userId},
		},
	}

	return shared.QueryAll[LibraryItem](containerName, userId, query, queryOptions)
}

func (r *LibraryRepository) GetById(id string, userId string) (LibraryItem, error) {
	return shared.QuerySingle[LibraryItem](containerName, id, userId)
}

func (r *LibraryRepository) DeleteById(id string, userId string) error {
	return shared.DeleteItem(containerName, id, userId)
}

func (r *LibraryRepository) Create(item LibraryItem, userId string) (string, error) {
	err := shared.UpsertItem(containerName, item, userId)
	return item.Id, err
}

func (r *LibraryRepository) Update(item LibraryItem, userId string) error {
	return shared.UpsertItem(containerName, item, userId)
}
