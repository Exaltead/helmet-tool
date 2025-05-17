package challengeAnswers

import (
	"haasteikko/backend/shared"
	"net/url"

	"github.com/Azure/azure-sdk-for-go/sdk/data/azcosmos"
)

const containerName string = "answers"

type ChallengeAnswerRepository struct{}

func (r *ChallengeAnswerRepository) GetAll(userId string, queryParams url.Values) ([]ChallengeAnswerSet, error) {
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
	if queryParams != nil && queryParams.Get("itemId") != "" {
		query += " AND c.itemId = @itemId"
		queryOptions.QueryParameters = append(queryOptions.QueryParameters, azcosmos.QueryParameter{
			Name:  "@itemId",
			Value: queryParams.Get("itemId")})
	}

	return shared.QueryAll[ChallengeAnswerSet](containerName, userId, query, queryOptions)
}

func (r *ChallengeAnswerRepository) GetById(id string, userId string) (ChallengeAnswerSet, error) {
	return shared.QuerySingle[ChallengeAnswerSet](containerName, id, userId)
}

func (r *ChallengeAnswerRepository) DeleteById(id string, userId string) error {
	return shared.DeleteItem(containerName, id, userId)
}

func (r *ChallengeAnswerRepository) Create(item ChallengeAnswerSet, userId string) (string, error) {
	err := shared.UpsertItem(containerName, item, userId)
	return item.Id, err
}

func (r *ChallengeAnswerRepository) Update(item ChallengeAnswerSet, userId string) error {
	return shared.UpsertItem(containerName, item, userId)
}
