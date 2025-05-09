export type Book = {
  kind: "Book"
  id: string
  name: string
  author: string
  translator?: string
  activatedChallengeIds: string[]
}

export type Entry = Book
