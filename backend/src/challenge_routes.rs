use axum::{
    extract::{Path, Query},
    response::Json,
    routing::{get, post},
    Router,
};
use serde::{Deserialize, Serialize};
use serde_json::{json, Value};

#[derive(Deserialize, Serialize)]
pub struct ChallengeQuestionParams {
    year: u32,
    question_set: String,
}

pub(crate) async fn get_questions(Query(params): Query<ChallengeQuestionParams>) -> Json<Value> {
    Json(json!({ "params": params }))
}
