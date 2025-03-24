import { z } from "zod"

export const questionSchema = z.object({
  id: z.number(),
  question: z.string(),
})

export type Question = z.infer<typeof questionSchema>
