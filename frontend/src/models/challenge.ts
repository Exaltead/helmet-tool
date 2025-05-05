import { z } from "zod"

export const questionTypeSchema = z.enum(["Boolean", "TextInput"])
export type QuestionType = z.infer<typeof questionTypeSchema>

export const questionSchema = z.object({
  kind: questionTypeSchema,
  id: z.string(),
  question: z.string(),
  number: z.number(),
  questionClusterSize: z.number(),
})

export type Question = z.infer<typeof questionSchema>

export const answerSchema = z.object({
  kind: questionTypeSchema,
  id: z.string(),
  questionId: z.string(),
  answered: z.boolean(),
  answer: z.enum(["yes", "no"]).or(z.string()),
  itemId: z.string(),
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

export const solutionSchema = z.object({
  questionId: z.string(),
  singleAnswerItemId: z.string(),
  multipleAnswerItemIds: z.string().array(),
})

export type Solution = z.infer<typeof solutionSchema>

export const solutionSetSchema = z.object({
  id: z.string().uuid(),
  challengeId: z.string().uuid(),
  solutions: solutionSchema.array(),
})

export type SolutionSet = z.infer<typeof solutionSetSchema>
