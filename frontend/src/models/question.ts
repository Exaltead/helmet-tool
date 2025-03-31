import { z } from "zod"

export const questionSchema = z.object({
  id: z.string(),
  question: z.string(),
})

export type Question = z.infer<typeof questionSchema>
