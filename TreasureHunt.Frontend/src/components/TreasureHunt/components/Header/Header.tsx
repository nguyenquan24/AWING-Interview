import React from 'react';
import { Box, Typography } from '@mui/material';
import ChestIcon from '@mui/icons-material/Lock';
import { headerStyles } from './styles';

const Header: React.FC = () => {
  return (
    <Box sx={headerStyles.container}>
      <Typography variant="h3" sx={headerStyles.title}>
        <ChestIcon sx={headerStyles.icon} />
        Treasure Hunt Solver
      </Typography>
      <Typography variant="subtitle1" color="text.secondary" sx={headerStyles.subtitle}>
        Find the optimal path to the treasure with minimum fuel consumption
      </Typography>
    </Box>
  );
};

export default Header; 