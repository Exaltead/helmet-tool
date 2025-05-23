package sharedchallenges

import (
	"haasteikko/backend/shared"

	"github.com/Azure/azure-sdk-for-go/sdk/data/azcosmos"
)

const sharedChallengesContainerName string = "challenges"

type SharedChallengeRepository struct {
}

func (r *SharedChallengeRepository) GetAll() ([]SharedChallenge, error) {
	query := "SELECT * FROM c"
	queryOptions := azcosmos.QueryOptions{}
	return shared.QueryAll[SharedChallenge](sharedChallengesContainerName, KindShared, query, queryOptions)
}

func (r *SharedChallengeRepository) GetById(id string) (SharedChallenge, error) {
	return shared.QuerySingle[SharedChallenge](sharedChallengesContainerName, id, KindShared)
}

func (r *SharedChallengeRepository) DeleteById(id string) error {
	return shared.DeleteItem(sharedChallengesContainerName, id, KindShared)
}

func (r *SharedChallengeRepository) Create(item SharedChallenge) (string, error) {
	err := shared.UpsertItem(sharedChallengesContainerName, item, KindShared)
	return item.Id, err
}

func (r *SharedChallengeRepository) Update(item SharedChallenge) error {
	return shared.UpsertItem(sharedChallengesContainerName, item, KindShared)
}
