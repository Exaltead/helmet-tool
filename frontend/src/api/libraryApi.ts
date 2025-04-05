import type { Book, Entry } from "@/models/entry"
import { getAccessToken } from "@/modules/auth-store"
import { z } from "zod"

const API_URL = import.meta.env.VITE_API_URL

const libraryBookSchema = z.object({
  title: z.string(),
  author: z.string(),
  translator: z.string().nullish(),
  //kanittaja: z.string().nullable()
})

const libraryItemSchema = z.object({
  id: z.string(),
  book: libraryBookSchema.optional(),
})

type ApiLibraryItem = z.infer<typeof libraryItemSchema>

const newApiLibraryItemSchema = libraryItemSchema.omit({ id: true })
type NewApiLibraryItem = z.infer<typeof newApiLibraryItemSchema>

export async function fetchLibraryItems(): Promise<Entry[]> {
  const resp = await fetch(`${API_URL}/library`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${getAccessToken()}`,
    },
  })

  if (!resp.ok) {
    throw new Error("Failed to fetch library items")
  }

  const data: ApiLibraryItem[] = libraryItemSchema.array().parse(await resp.json())

  const books = data
    .filter((item) => item.book !== undefined)
    .map(
      (item) =>
        ({
          kind: "Book",
          id: item.id,
          name: item.book!.title,
          author: item.book!.author,
          translator: item.book?.translator ? item.book?.translator : undefined,
        }) satisfies Book,
    )

  // As there are right now no other types of entries, we can just return the books
  return books
}

export async function addLibraryItem(item: Omit<Entry, "id">): Promise<void> {
  try {
    const newLibraryItem: NewApiLibraryItem = {
      book: {
        title: item.name,
        author: item.author,
        translator: item.translator, // null //item.translator ? item.translator : null,
      },
    }

    const validatedItem = newApiLibraryItemSchema.parse(newLibraryItem)
    const body = JSON.stringify(validatedItem)
    console.log("Adding library item", body)

    console.dir(validatedItem, { depth: null })

    const resp = await fetch(`${API_URL}/library`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${getAccessToken()}`,
      },
      body: body,
    })

    if (!resp.ok) {
      throw new Error("Failed to add library item")
    }
  } catch (e) {
    console.error("Error adding library item", e)
  }
}
