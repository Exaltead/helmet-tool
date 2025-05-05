import { solutionSetSchema, type SolutionSet } from "@/models/challenge"
import { BaseApiClient } from "./baseApiClient"

const newSolutionSetSchema = solutionSetSchema.omit({ id: true })

class SolutionsApiClient extends BaseApiClient<
  typeof solutionSetSchema,
  typeof newSolutionSetSchema
> {
  constructor() {
    super(solutionSetSchema, newSolutionSetSchema, "solution")
  }

  async fetchSolutionSets(): Promise<SolutionSet[]> {
    return this.fetchEntities(new URLSearchParams({}))
  }

  async addSolutionSet(solutionSet: Omit<SolutionSet, "id">): Promise<string> {
    return this.addEntity(solutionSet)
  }

  async updateSolutionSet(solutionSet: SolutionSet): Promise<void> {
    return this.updateEntity(solutionSet.id, solutionSet)
  }

  async getSolutionSetByChallengeId(challengeId: string): Promise<SolutionSet | undefined> {
    try {
      const solutionSets = await this.fetchEntities(new URLSearchParams({ challengeId }))
      return solutionSets[0]
    } catch {
      // TODO: map other than 404 to actual error
      return undefined
    }
  }
}

export const solutionsApiClient = new SolutionsApiClient()
