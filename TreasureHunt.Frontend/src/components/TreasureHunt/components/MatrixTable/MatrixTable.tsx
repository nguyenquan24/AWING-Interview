import React from 'react';
import {
  Card,
  CardContent,
  Typography,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableRow,
  TextField,
  Tooltip
} from '@mui/material';
import { matrixTableStyles, inputStyle } from './styles';

interface MatrixTableProps {
  matrix: number[][];
  maxValue: number;
  onCellChange: (row: number, col: number, value: string) => void;
}

const MatrixTable: React.FC<MatrixTableProps> = ({
  matrix,
  maxValue,
  onCellChange
}) => {
  return (
    <Card elevation={3} sx={matrixTableStyles.card}>
      <CardContent>
        <Typography variant="h6" gutterBottom sx={matrixTableStyles.title}>
          Treasure Map Matrix
        </Typography>
        <TableContainer>
          <Table size="small">
            <TableBody>
              {matrix.map((row, i) => (
                <TableRow key={i}>
                  {row.map((cell, j) => (
                    <TableCell 
                      key={j} 
                      padding="none" 
                      sx={matrixTableStyles.cell}
                    >
                      <Tooltip title={`Position (${i+1}, ${j+1})`} arrow>
                        <TextField
                          type="number"
                          value={cell || ''}
                          onChange={(e) => onCellChange(i, j, e.target.value)}
                          inputProps={{ 
                            min: 1, 
                            max: maxValue,
                            style: inputStyle
                          }}
                          variant="outlined"
                          size="small"
                          error={cell > maxValue || cell < 1}
                          sx={matrixTableStyles.textField}
                        />
                      </Tooltip>
                    </TableCell>
                  ))}
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </CardContent>
    </Card>
  );
};

export default MatrixTable; 