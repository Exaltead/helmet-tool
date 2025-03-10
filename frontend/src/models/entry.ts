export type Book = {
  kind: 'Book'
  name: string
  author: string
  translator?: string
}

export type Entry = Book
