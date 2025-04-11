import type { Challenge } from "@/models/challenge"
import { getAccessToken } from "@/modules/auth-store"

const API_URL = import.meta.env.VITE_API_URL

function getHeaders(): HeadersInit {
  return {
    "Content-Type": "application/json",
    Authorization: `Bearer ${getAccessToken()}`,
  }
}

export async function addChallenge(challenge: Challenge): Promise<void> {
  const resp = await fetch(`${API_URL}/challenge`, {
    method: "POST",
    headers: getHeaders(),
    body: JSON.stringify(challenge),
  })

  if (!resp.ok) {
    throw new Error("Failed to add challenge")
  }
}
