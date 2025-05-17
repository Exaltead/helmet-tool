import { challengeSchema, type Challenge } from "@/models/challenge"
import { getAccessToken } from "@/modules/auth-store"
import { z } from "zod"

const API_URL = import.meta.env.VITE_API_URL

function getHeaders(): HeadersInit {
  return {
    "Content-Type": "application/json",
    Authorization: `Bearer ${getAccessToken()}`,
  }
}

export async function fetchChallenges(): Promise<Challenge[]> {
  const resp = await fetch(`${API_URL}/challenge`, {
    method: "GET",
    headers: getHeaders(),
  })

  if (!resp.ok) {
    throw new Error("Failed to fetch challenges")
  }

  const data = await resp.json()
  return challengeSchema.array().parse(data)
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

export async function updateChallenge(challenge: Challenge): Promise<void> {
  const resp = await fetch(`${API_URL}/challenge/${challenge.id}`, {
    method: "PUT",
    headers: getHeaders(),
    body: JSON.stringify(challenge),
  })

  if (!resp.ok) {
    throw new Error("Failed to update challenge")
  }
}
