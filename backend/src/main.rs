use axum::{routing::get, Router};
use lambda_http::{run, service_fn, tracing, Error};
mod http_handler;
use http_handler::function_handler;
mod challenge_routes;
use challenge_routes::get_questions;

#[tokio::main]
async fn main() -> Result<(), Error> {
    tracing::init_default_subscriber();

    let app = Router::new().route("/challenge-questions", get(get_questions));

    run(app).await
}
