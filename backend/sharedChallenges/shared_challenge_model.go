package sharedchallenges

type Question struct {
	Kind                string `json:"kind"`
	Question            string `json:"question"`
	Id                  string `json:"id"`
	Number              int    `json:"number"`
	QuestionClusterSize int    `json:"questionClusterSize"`
}

type SharedChallenge struct {
	Id          string `json:"id"`
	Name        string `json:"name"`
	Status      string `json:"status"`
	TargetMedia string `json:"targetMedia"`
	Questions   []Question `json:"questions"`
	// Add other fields as needed
}

func (s SharedChallenge) GetId() string {
	return s.Id
}
