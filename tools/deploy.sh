#!/bin/bash

# Replace with your own values or pass them as arguments
STACK_NAME="nuget-codeartifact-stack"
TEMPLATE_FILE="../src/CloudFormation.yaml"
REPOSITORY_NAME="nuget-repo"
DOMAIN_NAME="improve"

# Deploy the CloudFormation stack
aws cloudformation deploy \
  --stack-name "$STACK_NAME" \
  --template-file "$TEMPLATE_FILE" \
  --capabilities CAPABILITY_NAMED_IAM \
  --parameter-overrides \
    RepositoryName="$REPOSITORY_NAME" \
    DomainName="$DOMAIN_NAME"
