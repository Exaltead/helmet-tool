import { z } from "zod"

const loginResponseSchema = z.object({
  token: z.string(),
})

const API_URL = import.meta.env.VITE_API_URL

export async function postLogin(username: string, password: string): Promise<string | undefined> {
  const url = `${API_URL}/login`
  const body = JSON.stringify({
    username,
    password,
  })
  const resp = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body,
  })

  if (!resp.ok) {
    return undefined
  }
  const data = loginResponseSchema.parse(await resp.json())

  return data.token
}
