use axum::{extract::Query, response::Json};
use serde::{Deserialize, Serialize};
use serde_json::{json, Value};

#[derive(Deserialize, Serialize)]
pub struct ChallengeQuestionParams {
    year: u32,
    question_set: String,
}

#[derive(Deserialize, Serialize)]
struct ChallengeQuestion {
    id: u32,
    question: String,
}

pub(crate) async fn get_questions(Query(_params): Query<ChallengeQuestionParams>) -> Json<Value> {
    let questions = vec![
        ChallengeQuestion {
            id: 1,
            question: "Oliko kirja hyvä?".to_string(),
        },
        ChallengeQuestion {
            id: 2,
            question: "Kuinka monta tähteä annat kirjalle?".to_string(),
        },
        ChallengeQuestion {
            id: 3,
            question: "Suosittelisitko kirjaa?".to_string(),
        },
    ];

    Json(json!(questions))
}
