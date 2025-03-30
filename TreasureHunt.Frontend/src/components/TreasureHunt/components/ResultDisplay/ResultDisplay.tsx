import React from 'react';
import { Alert, Box, Typography, Zoom } from '@mui/material';
import { resultDisplayStyles } from './styles';

interface ResultDisplayProps {
  result: number;
}

const ResultDisplay: React.FC<ResultDisplayProps> = ({ result }) => {
  return (
    <Zoom in>
      <Alert 
        severity="success"
        variant="filled"
        sx={resultDisplayStyles.alert}
      >
        <Box sx={resultDisplayStyles.container}>
          <Typography variant="h6" component="div" sx={resultDisplayStyles.title}>
            Minimum Fuel Required
          </Typography>
          <Typography variant="h5" sx={resultDisplayStyles.value}>
            {result.toFixed(5)}
          </Typography>
        </Box>
      </Alert>
    </Zoom>
  );
};

export default ResultDisplay; 