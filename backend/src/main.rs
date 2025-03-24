use axum::{routing::get, Router};
use lambda_http::{run, tracing, Error};
mod challenge_routes;
use challenge_routes::get_questions;

#[tokio::main]
async fn main() -> Result<(), Error> {
    tracing::init_default_subscriber();

    let app = Router::new().route("/challenge-questions", get(get_questions));

    run(app).await
}
