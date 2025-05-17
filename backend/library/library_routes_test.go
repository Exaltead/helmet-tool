package library

import (
	"testing"

	"github.com/stretchr/testify/assert"
)

func createTestBook() LibraryBook {
	return LibraryBook{
		Title:      "Test Book",
		Author:     "Test Author",
		Translator: nil,
	}
}

func TestCreateNewFromParsedItem(t *testing.T) {
	t.Run("Test createNewFromParsedItem", func(t *testing.T) {
		book := createTestBook()
		item := LibraryItem{
			Kind: "Book",
			Book: &book,
		}

		item, err := createNewFromParsedItem(item, "testUserId")
		assert.NoError(t, err)
		assert.Equal(t, "testUserId", item.UserId)
		assert.NotEmpty(t, item.Id)
		assert.NotEmpty(t, item.AddDate)
		assert.NotNil(t, item.ActivatedChallengeIds)
	})
}
func TestValidateItem(t *testing.T) {
	t.Run("Test validateItem", func(t *testing.T) {
		book := createTestBook()
		item := LibraryItem{
			Kind: "Book",
			Book: &book,
			ActivatedChallengeIds: []string{"challenge1", "challenge2"},
		}

		err := validateItem(item)
		assert.NoError(t, err)
	})

	t.Run("Test validateItem with nil Book", func(t *testing.T) {
		item := LibraryItem{
			Kind: "Book",
			Book: nil,
		}

		err := validateItem(item)
		assert.Error(t, err)
	})
}
