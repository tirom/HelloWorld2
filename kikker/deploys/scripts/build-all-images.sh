#!/bin/bash
TAG=${TAG:=$(git rev-parse --short HEAD)}
NAMESPACE=${NAMESPACE:="vndg"}
echo "${NAMESPACE} and ${TAG}"

#echo "Build MSSQLDb..."
#docker build -f deploys/dockers/mssqldb/Dockerfile -t vndg/cs-mssqldb:$TAG -t vndg/cs-mssqldb:latest .

echo "Build MySQL..."
docker build -f deploys/dockers/mysqldb/Dockerfile -t vndg/cs-mysqldb:$TAG -t vndg/cs-mysqldb:latest .

echo "Build IdP..."
docker build -f src/services/idp/Dockerfile -t vndg/cs-idp-service:$TAG -t vndg/cs-idp-service:latest .

echo "Build Inventory..."
docker build -f src/services/inventory/Dockerfile -t vndg/cs-inventory-service:$TAG -t vndg/cs-inventory-service:latest .

echo "Build Catalog..."
docker build -f src/services/catalog/Dockerfile -t vndg/cs-catalog-service:$TAG -t vndg/cs-catalog-service:latest .

echo "Build Rating..."
docker build -f src/services/rating/Dockerfile -t vndg/cs-rating-service:$TAG -t vndg/cs-rating-service:latest .

echo "Build Cart..."
docker build -f src/services/cart/Dockerfile -t vndg/cs-cart-service:$TAG -t vndg/cs-cart-service:latest .

echo "Build Review..."
docker build -f src/services/review/Dockerfile -t vndg/cs-review-service:$TAG -t vndg/cs-review-service:latest .

echo "Build SPA..."
docker build -f src/web/Dockerfile -t vndg/cs-spa:$TAG -t vndg/cs-spa:latest .

#docker rmi $(docker images -f "dangling=true" -q)