import { z } from "zod"

export const questionSchema = z.object({
  id: z.string(),
  question: z.string(),
  number: z.number(),
})

export type Question = z.infer<typeof questionSchema>

export const answerSchema = z.object({
  kind: z.literal("Boolean"),
  id: z.string(),
  questionId: z.string(),
  answered: z.boolean(),
  answer: z.enum(["yes", "no"]),
})

export type Answer = z.infer<typeof answerSchema>

export const challengeSchema = z.object({
  id: z.string(),
  name: z.string(),
  status: z.enum(["active", "inactive"]),
  targetMedia: z.enum(["Book"]),
  questions: questionSchema.array(),
})
export type Challenge = z.infer<typeof challengeSchema>
