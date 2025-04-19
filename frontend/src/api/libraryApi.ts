import type { Entry } from "@/models/entry"
import { getAccessToken } from "@/modules/auth-store"
import { z } from "zod"

const API_URL = import.meta.env.VITE_API_URL

const libraryBookSchema = z.object({
  title: z.string(),
  author: z.string(),
  translator: z.string().nullish(),
})

const libraryItemSchema = z.object({
  id: z.string(),
  book: libraryBookSchema.optional(),
})

type ApiLibraryItem = z.infer<typeof libraryItemSchema>

const newApiLibraryItemSchema = libraryItemSchema.omit({ id: true })
type NewApiLibraryItem = z.infer<typeof newApiLibraryItemSchema>

function getHeaders(): HeadersInit {
  return {
    "Content-Type": "application/json",
    Authorization: `Bearer ${getAccessToken()}`,
  }
}

function mapApiLibraryItem(item: ApiLibraryItem): Entry {
  if (item.book !== undefined) {
    return {
      kind: "Book",
      id: item.id,
      name: item.book!.title,
      author: item.book!.author,
      translator: item.book?.translator ? item.book?.translator : undefined,
      activatedChallengeIds: [],
    }
  }
  throw new Error("Item type not supported yet")
}

export async function fetchLibraryItems(): Promise<Entry[]> {
  const resp = await fetch(`${API_URL}/library`, {
    method: "GET",
    headers: getHeaders(),
  })

  if (!resp.ok) {
    throw new Error("Failed to fetch library items")
  }

  const data: ApiLibraryItem[] = libraryItemSchema.array().parse(await resp.json())

  return data.filter((item) => item.book !== undefined).map((t) => mapApiLibraryItem(t))
}

export async function addLibraryItem(item: Omit<Entry, "id">): Promise<string | undefined> {
  try {
    const newLibraryItem: NewApiLibraryItem = {
      book: {
        title: item.name,
        author: item.author,
        translator: item.translator,
      },
    }

    const validatedItem = newApiLibraryItemSchema.parse(newLibraryItem)
    const body = JSON.stringify(validatedItem)

    const resp = await fetch(`${API_URL}/library`, {
      method: "POST",
      headers: getHeaders(),
      body: body,
    })

    if (!resp.ok) {
      throw new Error("Failed to add library item")
    }

    const data = z
      .object({
        id: z.string(),
      })
      .parse(await resp.json())
    return data.id
  } catch (e) {
    console.error("Error adding library item", e)
  }
}

export async function deleteLibraryItem(id: string): Promise<void> {
  const resp = await fetch(`${API_URL}/library/${id}`, {
    method: "DELETE",
    headers: getHeaders(),
  })

  if (!resp.ok) {
    throw new Error("Failed to delete library item")
  }
}

export async function updateLibraryItem(item: Entry): Promise<void> {
  const apiItem: ApiLibraryItem = {
    id: item.id,
    book: {
      title: item.name,
      author: item.author,
      translator: item.translator,
    },
  }

  const validatedItem = libraryItemSchema.parse(apiItem)
  const body = JSON.stringify(validatedItem)
  const resp = await fetch(`${API_URL}/library/${item.id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${getAccessToken()}`,
    },
    body: body,
  })
  if (!resp.ok) {
    throw new Error("Failed to update library item")
  }
}

export async function fetchLibraryItem(id: string): Promise<Entry> {
  const resp = await fetch(`${API_URL}/library?itemId=${id}`, {
    method: "GET",
    headers: getHeaders(),
  })
  if (!resp.ok) {
    throw new Error("Failed to fetch library item")
  }
  const data: ApiLibraryItem[] = libraryItemSchema.array().parse(await resp.json())
  const selected = data[0]
  if (selected === undefined) {
    throw new Error("Item not found")
  }

  return mapApiLibraryItem(selected)
}
