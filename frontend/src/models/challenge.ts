import { z } from "zod"
import { questionSchema } from "./question"

export const challengeSchema = z.object({
  id: z.string(),
  name: z.string(),
  status: z.enum(["active", "inactive"]),
  targetMedia: z.enum(["Book"]),
  questions: questionSchema.array(),
})
export type Challenge = z.infer<typeof challengeSchema>
