import { SxProps, Theme } from '@mui/material';

interface MatrixTableStyles {
  card: SxProps<Theme>;
  title: SxProps<Theme>;
  cell: SxProps<Theme>;
  textField: SxProps<Theme>;
}

export const matrixTableStyles: MatrixTableStyles = {
  card: {
    borderRadius: 2
  },
  title: {
    mb: 2
  },
  cell: {
    width: 60,
    height: 60,
    border: '1px solid rgba(224, 224, 224, 1)'
  },
  textField: {
    width: '100%',
    '& .MuiOutlinedInput-root': {
      '& fieldset': {
        border: 'none'
      }
    }
  }
};

export const inputStyle = {
  textAlign: 'center' as const,
  fontSize: '1rem',
  padding: '8px'
}; 