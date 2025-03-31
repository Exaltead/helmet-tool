import { questionSchema, type Question } from "@/models/question"
import { getAccessToken } from "@/modules/auth-store"

const API_URL = import.meta.env.VITE_API_URL

export async function fetchQuestions(): Promise<Question[]> {
  const resp = await fetch(`${API_URL}/GetChallenges`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${getAccessToken()}`,
    },
  })
  const data = questionSchema.array().parse(await resp.json())

  return data
}
