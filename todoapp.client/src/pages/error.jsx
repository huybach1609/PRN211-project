import React from 'react';
import { AlertCircle, RefreshCw, Home } from 'lucide-react';
import { Button } from '@heroui/react';

const ErrorPage = ({ errorType = "configuration", message = "An error occurred" }) => {
  const handleRefresh = () => {
    window.location.reload();
  };

  const handleGoHome = () => {
    window.location.href = '/';
  };

  const getErrorMessage = () => {
    switch (errorType) {
      case 'configuration':
        return {
          title: "Configuration Error",
          description: "The application is missing required environment variables. Please check your configuration.",
          details: "VITE_API_URL environment variable is not set. Please contact your administrator or check your environment configuration."
        };
      case 'network':
        return {
          title: "Network Error", 
          description: "Unable to connect to the server. Please check your internet connection.",
          details: "The application cannot reach the backend API server."
        };
      default:
        return {
          title: "Error",
          description: message,
          details: "An unexpected error has occurred."
        };
    }
  };

  const errorInfo = getErrorMessage();

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50">
      <div className="max-w-md w-full bg-white rounded-lg shadow-lg p-8 text-center">
        <div className="flex justify-center mb-6">
          <AlertCircle className="h-16 w-16 text-red-500" />
        </div>
        
        <h1 className="text-2xl font-bold text-gray-900 mb-4">
          {errorInfo.title}
        </h1>
        
        <p className="text-gray-600 mb-4">
          {errorInfo.description}
        </p>
        
        <div className="bg-gray-100 rounded-lg p-4 mb-6 text-left">
          <p className="text-sm text-gray-700">
            {errorInfo.details}
          </p>
        </div>
        
        <div className="space-y-3">
          <Button
            onClick={handleRefresh}
            className="w-full bg-blue-600 text-white"
            startContent={<RefreshCw className="h-4 w-4" />}
          >
            Refresh Page
          </Button>
          
          <Button
            onClick={handleGoHome}
            variant="bordered"
            className="w-full"
            startContent={<Home className="h-4 w-4" />}
          >
            Go to Home
          </Button>
        </div>
      </div>
    </div>
  );
};

export default ErrorPage;
