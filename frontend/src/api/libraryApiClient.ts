import { z } from "zod";
import { BaseApiClient } from "./baseApiClient";
import type { Book, Game, LibraryItem } from "@/models/LibraryItem";

const libraryBookSchema = z.object({
  title: z.string(),
  author: z.string(),
  translator: z.string().optional(),
})

const libraryGameSchema = z.object({
  title: z.string(),
  creator: z.string()
})

const libraryApiItemSchema = z.object({
  kind: z.literal("Book").or(z.literal("Game")),
  id: z.string(),
  book: libraryBookSchema.optional(),
  game: libraryGameSchema.optional(),
  activatedChallengeIds: z.string().array(),
  favorite: z.boolean()
})

type ApiLibraryItem = z.infer<typeof libraryApiItemSchema>

const newApiLibraryItemSchema = libraryApiItemSchema.omit({ id: true })
type NewApiLibraryItem = z.infer<typeof newApiLibraryItemSchema>

function mapFromApi(item: ApiLibraryItem): LibraryItem {
  switch (item.kind) {
    case "Book":
      const book: Book = {
        kind: "Book",
        favorite: item.favorite,
        id: item.id,
        activatedChallengeIds: item.activatedChallengeIds,
        translator: item.book?.translator,
        title: item.book!.title,
        author: item.book!.author
      }

      return book
    case "Game":
      const game: Game = {
        kind: "Game",
        favorite: item.favorite,
        id: item.id,
        activatedChallengeIds: item.activatedChallengeIds,
        title: item.game!.title,
        creator: item.game!.creator
      }

      return game
  }
}

function mapToApi(item: LibraryItem): NewApiLibraryItem {

  switch (item.kind) {
    case "Book":

      return {
        kind: "Book",
        book: {
          title: item.title,
          author: item.author,
          translator: item.translator ?? "",
        },
        activatedChallengeIds: item.activatedChallengeIds,
        favorite: item.favorite,
      };
    case "Game":
      return {
        kind: "Game",
        game: {
          title: item.title,
          creator: item.creator,
        },
        activatedChallengeIds: item.activatedChallengeIds,
        favorite: item.favorite,
      };
  }
}

class LibraryApiClient extends BaseApiClient<
  typeof libraryApiItemSchema,
  typeof newApiLibraryItemSchema> {
    constructor(){
      super(libraryApiItemSchema, newApiLibraryItemSchema, "library")
    }

    async fetchLibraryItems(): Promise<LibraryItem[]> {
      const items = await this.fetchEntities(new URLSearchParams())
      return items.map(mapFromApi)
    }

    async addLibraryItem(item: Omit<LibraryItem, "id">): Promise<string> {
      // Discriminated unions and omits do not work together
      const apiItem = mapToApi({...item } as unknown as LibraryItem)
      return this.addEntity(apiItem)
    }

    async updateLibraryItem(item: LibraryItem): Promise<void> {
      const apiItem = mapToApi(item)
      return await this.updateEntity(item.id, {...apiItem, id: item.id})
    }

    async deleteItem(id: string): Promise<void>{
      return await this.deleteEntity(id)
    }

    async getLibraryItem(id: string): Promise<LibraryItem | undefined> {
      const item = await this.fetchEntity(id)
      if(item === undefined) {
        return undefined
      }

      return mapFromApi(item)
    }
}


export const libraryApi = new LibraryApiClient()
