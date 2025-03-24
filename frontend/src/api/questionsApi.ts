import { questionSchema, type Question } from "@/models/question"

const API_URL = import.meta.env.VITE_API_URL

export async function fetchQuestions(): Promise<Question[]> {
  const resp = await fetch(`${API_URL}/challenge-questions?year=2025&question_set=book`)
  const data = questionSchema.array().parse(await resp.json())

  return data
}
