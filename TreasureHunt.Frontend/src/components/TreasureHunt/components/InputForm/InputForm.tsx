import React from 'react';
import { Box, Card, TextField } from '@mui/material';
import { inputFormStyles } from './styles';

interface InputFormProps {
  n: number;
  m: number;
  p: number;
  onChangeN: (value: number) => void;
  onChangeM: (value: number) => void;
  onChangeP: (value: number) => void;
}

const InputForm: React.FC<InputFormProps> = ({
  n, m, p,
  onChangeN,
  onChangeM,
  onChangeP
}) => {
  return (
    <Card elevation={3} sx={inputFormStyles.card}>
      <Box sx={inputFormStyles.container}>
        <Box sx={inputFormStyles.inputContainer}>
          <TextField
            label="Rows (N)"
            type="number"
            value={n}
            onChange={(e) => onChangeN(Math.max(1, Math.min(500, Number(e.target.value))))}
            fullWidth
            inputProps={{ min: 1, max: 500 }}
            variant="outlined"
            helperText="Maximum 500 rows"
          />
        </Box>
        <Box sx={inputFormStyles.inputContainer}>
          <TextField
            label="Columns (M)"
            type="number"
            value={m}
            onChange={(e) => onChangeM(Math.max(1, Math.min(500, Number(e.target.value))))}
            fullWidth
            inputProps={{ min: 1, max: 500 }}
            variant="outlined"
            helperText="Maximum 500 columns"
          />
        </Box>
        <Box sx={inputFormStyles.inputContainer}>
          <TextField
            label="Max Chest Number (P)"
            type="number"
            value={p}
            onChange={(e) => onChangeP(Math.max(1, Math.min(n * m, Number(e.target.value))))}
            fullWidth
            inputProps={{ min: 1, max: n * m }}
            variant="outlined"
            helperText={`Maximum ${n * m} chests`}
          />
        </Box>
      </Box>
    </Card>
  );
};

export default InputForm; 