mkdir -p deployment
GOOS=linux GOARCH=amd64 go build -o backend
mv backend deployment/backend
cp -r api deployment/
cp host.json deployment/host.json

cd deployment

sed -i 's/\.exe//g' host.json
rm function.zip
zip -r function.zip .
az functionapp deployment source config-zip \
  --src function.zip \
  --name <your-function-app> \
  --resource-group <your-resource-group>