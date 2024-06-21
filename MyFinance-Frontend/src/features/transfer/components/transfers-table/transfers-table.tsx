import { DataTable } from '@/components/ui/data-table'

import { Transfer } from '../../models/transfer'
import { RegisterTransferDialog } from '../register-transfer-dialog'
import { columns } from './transfers-table-column'

const transfers: Transfer[] = [
  {
    id: '1',
    relatedTo: '2',
    value: 100,
    settlementDate: new Date(),
    type: 1,
    accountTag: '3',
    category: '4',
  },
  {
    id: '5',
    relatedTo: '6',
    value: 200,
    settlementDate: new Date(),
    type: 2,
    accountTag: '8',
    category: '9',
  },
  {
    id: '10',
    relatedTo: '11',
    value: 300,
    settlementDate: new Date(),
    type: 2,
    accountTag: '13',
    category: '14',
  },
  {
    id: '15',
    relatedTo: '16',
    value: 400,
    settlementDate: new Date(),
    type: 1,
    accountTag: '18',
    category: '19',
  },
  {
    id: '20',
    relatedTo: '21',
    value: 500,
    settlementDate: new Date(),
    type: 2,
    accountTag: '23',
    category: '24',
  },
  {
    id: '25',
    relatedTo: '26',
    value: 600,
    settlementDate: new Date(),
    type: 1,
    accountTag: '28',
    category: '29',
  },
  {
    id: '30',
    relatedTo: '31',
    value: 700,
    settlementDate: new Date(),
    type: 2,
    accountTag: '33',
    category: '34',
  },
  {
    id: '35',
    relatedTo: '36',
    value: 800,
    settlementDate: new Date(),
    type: 1,
    accountTag: '38',
    category: '39',
  },
  {
    id: '40',
    relatedTo: '41',
    value: 900,
    settlementDate: new Date(),
    type: 1,
    accountTag: '43',
    category: '44',
  },
  {
    id: '45',
    relatedTo: '46',
    value: 1000,
    settlementDate: new Date(),
    type: 1,
    accountTag: '48',
    category: '49',
  },
  {
    id: '50',
    relatedTo: '51',
    value: 1100,
    settlementDate: new Date(),
    type: 1,
    accountTag: '53',
    category: '54',
  },
  {
    id: '55',
    relatedTo: '56',
    value: 1200,
    settlementDate: new Date(),
    type: 1,
    accountTag: '58',
    category: '59',
  },
  {
    id: '60',
    relatedTo: '61',
    value: 1300,
    settlementDate: new Date(),
    type: 2,
    accountTag: '63',
    category: '64',
  },
  {
    id: '65',
    relatedTo: '66',
    value: 1400,
    settlementDate: new Date(),
    type: 2,
    accountTag: '68',
    category: '69',
  },
  {
    id: '70',
    relatedTo: '71',
    value: 1500,
    settlementDate: new Date(),
    type: 2,
    accountTag: '73',
    category: '74',
  },
  {
    id: '75',
    relatedTo: '76',
    value: 1600,
    settlementDate: new Date(),
    type: 2,
    accountTag: '78',
    category: '79',
  },
  {
    id: '80',
    relatedTo: '81',
    value: 1700,
    settlementDate: new Date(),
    type: 2,
    accountTag: '83',
    category: '84',
  },
  {
    id: '85',
    relatedTo: '86',
    value: 1800,
    settlementDate: new Date(),
    type: 2,
    accountTag: '88',
    category: '89',
  },
  {
    id: '90',
    relatedTo: '91',
    value: 1900,
    settlementDate: new Date(),
    type: 2,
    accountTag: '93',
    category: '94',
  },
  {
    id: '95',
    relatedTo: '96',
    value: 2000,
    settlementDate: new Date(),
    type: 2,
    accountTag: '98',
    category: '99',
  },
]

export function TransfersTable() {
  const data = transfers.map((transfer) => {
    const formattedValue =
      transfer.type === 1 ? transfer.value : -1 * transfer.value
    return {
      ...transfer,
      value: formattedValue,
    } as Transfer
  })

  return (
    <DataTable columns={columns} data={data}>
      <RegisterTransferDialog />
    </DataTable>
  )
}
