package challengeSolutions

import (
	"haasteikko/backend/shared"
	"net/url"

	"github.com/Azure/azure-sdk-for-go/sdk/data/azcosmos"
)

const containerName string = "solutions"

type SolutionRepository struct{}

func (r *SolutionRepository) GetAll(userId string, queryParams url.Values) ([]SolutionSet, error) {
	query := "SELECT * FROM c WHERE c.userId = @userId"
	queryOptions := azcosmos.QueryOptions{
		QueryParameters: []azcosmos.QueryParameter{
			{Name: "@userId", Value: userId},
		},
	}

	if queryParams != nil && queryParams.Get("challengeId") != "" {
		query += " AND c.challengeId = @challengeId"
		queryOptions.QueryParameters = append(queryOptions.QueryParameters, azcosmos.QueryParameter{
			Name:  "@challengeId",
			Value: queryParams.Get("challengeId"),
		})
	}

	return shared.QueryAll[SolutionSet](containerName, userId, query, queryOptions)
}

func (r *SolutionRepository) GetById(id string, userId string) (SolutionSet, error) {
	return shared.QuerySingle[SolutionSet](containerName, id, userId)
}

func (r *SolutionRepository) DeleteById(id string, userId string) error {
	return shared.DeleteItem(containerName, id, userId)
}

func (r *SolutionRepository) Create(item SolutionSet, userId string) (string, error) {
	err := shared.UpsertItem(containerName, item, userId)
	return item.Id, err
}

func (r *SolutionRepository) Update(item SolutionSet, userId string) error {
	return shared.UpsertItem(containerName, item, userId)
}
