package auth

import (
	"fmt"
	"haasteikko/backend/shared"

	"github.com/Azure/azure-sdk-for-go/sdk/data/azcosmos"
)

type UserInfo struct {
	Username     string `json:"username"`
	PasswordHash string `json:"passwordHash"`
	Id           string `json:"id"`
}

func getUserInfo(username string) (*UserInfo, error) {
	containerName := "users"

	container, err := shared.ConnectToCosmosContainer(containerName)
	if err != nil {
		return nil, err
	}

	query := "SELECT * FROM c WHERE c.username = @username"
	queryOptions := azcosmos.QueryOptions{
		QueryParameters: []azcosmos.QueryParameter{
			{Name: "@username", Value: username},
		},
	}

	pager := container.NewQueryItemsPager(query, azcosmos.NewPartitionKey(), &queryOptions)
	if users, err := shared.LoadDataFromPager[UserInfo](pager); err == nil {
		if len(users) == 1 {
			return &users[0], nil
		}
	} else {
		fmt.Println(err)

	}

	return nil, nil

}
